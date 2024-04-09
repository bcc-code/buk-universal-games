/** Default time offset is 5 minutes before match starts */
export default (notificationService, timeOffset = -300_000) => (store) => {
  // called when the store is initialized
  store.subscribe(async (mutation, state) => {
    if (mutation.type === 'setMatches') {
      for (let match of mutation.payload) {
        const time = new Date(new Date().setHours(...match.start.split(":").concat([0, 0])) + timeOffset)
        const game = state.games.find(g => g.id === match.gameId);
        notificationService.schedule(time, `${match.start} ${game.name}`, { body: `${match.team1} — ${match.team2}`, icon: `/icon/game-${game.gameType.replaceAll("_","")}.svg` })
      }
    }
  })
}
