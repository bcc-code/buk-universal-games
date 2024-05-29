export function formatPoints(points: number | undefined): string {
  if (points === undefined) {
    return '£ ?';
  }

  if (points < 1000) {
    return `£ ${points.toFixed(0)}k`;
  } else {
    return `£ ${(points / 1000).toFixed(2)}M`;
  }
}
