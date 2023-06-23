export default class NotificationService {
  scheduledNotifications = [
    {
      time: new Date(new Date().getTime() + 10_000),
      title: 'Test',
      options: {
        body: 'This is a scheduled test notification',
        icon: 'images/ubg-logo.png',
      }
    }
  ];
  get canNotifyExternal() {
    return "Notification" in window && Notification.permission === "granted";
  }
  get canAskForPermission() {
    return "Notification" in window && Notification.permission === "default";
  }
  constructor() {
    setInterval(() => {
      let latestNotification;
      while (this.scheduledNotifications[0]?.time < new Date()) {
        latestNotification = this.scheduledNotifications.shift();
      }
      if (latestNotification) {
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
