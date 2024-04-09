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
  shownNotifications = [];
  get canNotifyExternal() {
    return navigator.serviceWorker && "Notification" in window && Notification.permission === "granted";
  }
  get canAskForPermission() {
    return "Notification" in window && Notification.permission === "default";
  }
  /** Scheduled notifications expire 5 minutes after their target time by default. */
  constructor(expiryTimeOffset = 300_000) {
    setInterval(() => {
      let latestNotification;
      while (this.scheduledNotifications[0]?.time < new Date()) {
        latestNotification = this.scheduledNotifications.shift();
      }
      if (latestNotification && !this.shownNotifications.find(n => n.time.getTime() === latestNotification.time.getTime())
        && new Date().getTime() < latestNotification.time.getTime() + expiryTimeOffset) {
        this.shownNotifications.push(latestNotification);
        this.notify(latestNotification.title, latestNotification.options);
      }
    }, 1000);
  }
  requestExternal() {
    // this.notifyInternal("Success!", {
    //   body: "System notifications have been enabled",
    //   icon: "image/ubg-logo.svg",
    // });
    if(navigator.serviceWorker && this.canAskForPermission)
    {
      Notification.requestPermission();
    }
  }
  notify(title, options) {
    this.canNotifyExternal
      ? this.notifyExternal(title, options)
      : this.notifyInternal(title, options);
  }
  schedule(time, title, options) {
    // Do not reschedule notifications that have already been shown.
    if (!this.shownNotifications.find(n => n.time.getTime() === time.getTime())) {
      this.scheduledNotifications.push({ time, title, options });
      this.scheduledNotifications = this.scheduledNotifications
        // Sort ascending
        .sort((a, b) => a.time.getTime() - b.time.getTime())
        // Distinct
        .filter((n, ix, all) => all.findIndex(n2 => n2.time.getTime() === n.time.getTime()) === ix);
    }
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
