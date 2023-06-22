export default (notificationService) => (store) => {
  // called when the store is initialized
  store.subscribe(async (mutation, state) => {
    console.log(mutation.type)
    if (mutation.type === 'setMatches') {
      for (let match of mutation.payload) {
        const time = new Date(new Date().setHours(...match.start.split(":")))
        const game = state.games.find(g => g.id === match.gameId);
        notificationService.schedule(time, `${match.start}: ${game.name}`, { body: `${match.team1} — ${match.team2}` })
        notificationService.notify(`${match.start}: ${game.name}`, { body: `${match.team1} — ${match.team2}` })
      }
    }
  })
}
