export default () => (store) => {
  store.subscribe(async (mutation) => {
    if (mutation.type === 'setMatches') {
      store.dispatch('checkNewQuestions');
      setInterval(() => {
        console.log('checking for new questions');
        store.dispatch('checkNewQuestions');
      }, 60 * 1000);
    }
  });
};
