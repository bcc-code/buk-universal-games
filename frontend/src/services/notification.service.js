export default class NotificationService {
  scheduledNotifications = [
    {
      time: new Date(new Date().getTime() + 10_000),
      title: 'Test',
      options: {
        body: 'This is a scheduled test notification',
        icon: 'images/ubg-logo.png'
      }
    }
  ];
  get canNotifyExternal() {
    return "Notification" in window && Notification.permission === "granted";
  }
  get dialogue() {
    let dialogue = document.getElementById("ubg-dialogue");
    if (!dialogue) {
      dialogue = document.createElement("dialog");
      dialogue.id = 'ubg-dialogue'
      const title = document.createElement("h2");
      const body = document.createElement("p");
      const icon = document.createElement("img");
      title.className = "title";
      body.className = "body";
      icon.className = "icon";
      dialogue.appendChild(title);
      dialogue.appendChild(body);
      dialogue.appendChild(icon);
      document.body.appendChild(dialogue);
      dialogue.addEventListener("click", () => {
        dialogue.style.visibility = "hidden";
        title.textContent = "";
        body.textContent = "";
        icon.style.visibility = "hidden";
        icon.src = "";
      });
    }
    return dialogue;
  }
  get canAskForPermission() {
    return "Notification" in window && Notification.permission === "default";
  }
  constructor() {
    setInterval(() => {
      let latestNotification;
      while (this.scheduledNotifications[0]?.time < new Date()) {
        latestNotification = this.scheduledNotifications.pop();
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
  notifyExternal(title, options) {
    const serviceWorkerRegistration = ServiceWorkerRegistration.active;
    if ('showNotification' in serviceWorkerRegistration) {
      serviceWorkerRegistration.showNotification(title, options)
    } else {
      new Notification(title, options);
    }
  }
  notifyInternal(title, options) {
    const dialogue = this.dialogue;
    dialogue.getElementsByClassName("title")[0].textContent = title;
    dialogue.getElementsByClassName("body")[0].textContent = options.body;
    if (options.image) {
      const image = dialogue.getElementsByClassName("image")[0];
      image.src = options.image;
      image.style.visibility = "visible";
    }
    dialogue.style.visibility = "visible";
  }
}
