import { Component, OnInit } from '@angular/core';
import { GameDetailsService } from '../shared/game-details.service';
import { GameDetails } from '../shared/game-details.model';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styles: ``
})
export class GameDetailsComponent  implements OnInit {
  grid: string[][] = [];
  statusMessage: string = '';
  totalHits: number = 0;
  totalMisses: number = 0;
  totalShots: number = 0;
  battleshipHits: number = 0;
  destroyer1Hits: number = 0;
  restartReq: boolean = false;

  xHeaders: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J']; // X-axis headers
  yHeaders: string[] = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10']; // Y-axis headers

  constructor(public gameDetailsService: GameDetailsService) {}

  ngOnInit(): void {
    this.loadGrid();
  }

  // Load the grid from the backend
  loadGrid(): void {
    this.gameDetailsService.getGrid().subscribe(
      (data: GameDetails) => {
        console.log(data);
        // Extract the grid and convert characters
        this.grid = data.grid.map((row) =>
          row.map((cell) => {
            if (cell === '~') return '🌊'; // Water
            if (cell === 'M') return 'M🌊'; // Miss (both 'M' and '🌊')
            if (cell === 'H') return '💥'; // Hit
            return cell; // Other cells (e.g., Ship 'S') will remain unchanged
          })
        );
  
        // Optionally, handle other properties from GameResponse
        this.totalHits = data.totalHits;
        this.totalMisses = data.totalMisses;
        this.totalShots = data.totalShots;
        this.battleshipHits = data.battleshipHits;
        this.destroyer1Hits = data.destroyer1Hits;

        if(this.battleshipHits == 1 && this.destroyer1Hits == 2)
        {
          if(this.restartReq == false)
          {
            const userConfirmed = window.confirm('You have won the game...! Do you want to restart?');
            if (userConfirmed) {
              this.restartGame();
              this.restartReq = false;
            }else{
              this.restartReq = true;
            }
          }
        }
      },
      (error) => {
        console.error('Error loading grid', error);
      }
    );
  }

  onCellClick(row: number, col: number): void {
    // Convert col to a letter (X-axis) and row to a number (Y-axis)
    const coordinate = `${String.fromCharCode(65 + col)}${row + 1}`;
  
    this.gameDetailsService.fireShot(coordinate).subscribe(
      (result) => {
        console.log(result);
        if(result.isSunk){
          const userConfirmed = window.confirm(result.resultMessage);
          if (userConfirmed) {
            this.loadGrid(); // Reload the grid to reflect changes
          } 
        }else {
          this.loadGrid(); // Reload the grid to reflect changes
        }
      },
      (error) => {
        console.error('Error firing shot', error);
      }
    );
  }  

  confirmRestart(): void {
    // Show confirmation dialog before restarting
    const userConfirmed = window.confirm('Are you sure you want to restart the game?');

    if (userConfirmed) {
      this.restartGame();
    }
  }

  restartGame(): void {
    // Call the service to reset the game
    this.gameDetailsService.restartGame().subscribe(
      (data: GameDetails) => {
        console.log(data);
        // Extract the grid and convert characters
        this.grid = data.grid.map((row) =>
          row.map((cell) => {
            if (cell === '~') return '🌊'; // Water
            if (cell === 'M') return 'M🌊'; // Miss (both 'M' and '🌊')
            if (cell === 'H') return '💥'; // Hit
            return cell; // Other cells (e.g., Ship 'S') will remain unchanged
          })
        );
  
        // Optionally, handle other properties from GameResponse
        this.totalHits = data.totalHits;
        this.totalMisses = data.totalMisses;
        this.totalShots = data.totalShots;
        this.battleshipHits = data.battleshipHits;
        this.destroyer1Hits = data.destroyer1Hits;
        this.restartReq = true;
        console.log('Game restarted:', data);
      },
      (error) => {
        console.error('Error restarting the game:', error);
      }
    );
  }
}
