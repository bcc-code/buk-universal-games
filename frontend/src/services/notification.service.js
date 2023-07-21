export const testNotification = {
  time: new Date(new Date().getTime() + 10_000),
  title: 'Test',
  options: {
    body: 'This is a scheduled test notification',
    icon: 'image/ubg-logo.svg',
  }
};

export default class NotificationService {
  scheduledNotifications = [];
  get canNotifyExternal() {
    return "Notification" in window && Notification.permission === "granted";
  }
  get canAskForPermission() {
    return "Notification" in window && Notification.permission === "default";
  }
  /** Scheduled notifications expire 5 minutes after their target time by default. */
  constructor(expiryTimeOffset = 300_000) {
    // Debug mode only
    if ('webpackChunkbuk_universal_games_ui' in window) {
      this.scheduledNotifications.push()
    }
    setInterval(() => {
      let latestNotification;
      while (this.scheduledNotifications[0]?.time < new Date()) {
        latestNotification = this.scheduledNotifications.shift();
      }
      if (new Date().getTime() < latestNotification?.time.getTime() + expiryTimeOffset) {
        this.notify(latestNotification.title, latestNotification.options);
      }
    }, 1000);
  }
  requestExternal() {
    Notification.requestPermission().then((result) => {
      if (result === "granted") {
        this.notifyExternal("Success!", {
          body: "System notifications have been enabled",
        });
      }
    });
  }
  notify(title, options) {
    this.canNotifyExternal
      ? this.notifyExternal(title, options)
      : this.notifyInternal(title, options);
  }
  schedule(time, title, options) {
    this.scheduledNotifications.push({ time, title, options });
    // Sort ascending
    this.scheduledNotifications.sort((a, b) => a.time.getTime() - b.time.getTime());
  }
  async notifyExternal(title, options) {
    const registration = await navigator.serviceWorker.getRegistration();
    if (registration && 'showNotification' in registration) {
      await registration.showNotification(title, { ...options, data: options.onClick });
    } else {
      new Notification(title, options);
    }
  }
  notifyInternal = () => {
    throw new Error('Please register a notifier!');
  }

  registerInternalNotifier(fn) {
    this.notifyInternal = fn;
  }
}
