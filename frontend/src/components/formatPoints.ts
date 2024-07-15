export function formatPoints(points: number | undefined): string {
  if (points === undefined) {
    return '£ ?';
  }

  const lire = points * 100000;

  if (lire < 1000000) {
    return `£ ${(lire / 1000).toFixed(0)}k`;
  } else {
    return `£ ${(lire / 1000000).toFixed(2)}M`;
  }
}
