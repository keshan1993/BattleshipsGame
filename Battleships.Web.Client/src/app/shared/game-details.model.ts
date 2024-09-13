export class GameDetails {
  grid: string[][]; // 2D array of strings to represent the grid
  totalHits: number;
  totalMisses: number;
  totalShots: number;
  battleshipHits: number;
  destroyer1Hits: number;
}

export class Shot {
    coordinate?: string;
    hit?: boolean;
    resultMessage?: string;
    isSunk?: boolean;
}
