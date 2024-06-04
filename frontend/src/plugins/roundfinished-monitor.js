export default () => (store) => {
  store.subscribe(async (mutation) => {
    if (mutation.type === 'setMatches') {
      store.dispatch('checkNewQuestions');
      console.log('Started checking for new questions');
      setInterval(() => {
        store.dispatch('checkNewQuestions');
      }, 60 * 1000);
    }
  });
};
