window.addEventListener('load', () => {
  if (Notification.permission !== 'granted') {
    const bell = document.createElement('div');
    bell.innerText = '🔔'
    bell.style.position = 'absolute';
    bell.style.top = bell.style.right = '1em';
    bell.style.cursor = 'pointer';
    bell.addEventListener('click', () => {
      Notification.requestPermission().then((result) => {
        if (result === 'granted') {
          bell.remove();
          new Notification('Success!', {
            body: 'Notifications have been enabled',
            img: 'icon/192.png'
          });
        }
      });
    });
    document.body.appendChild(bell);
  }
});
