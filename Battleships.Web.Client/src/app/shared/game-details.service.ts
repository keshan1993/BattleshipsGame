import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GameDetails, Shot } from './game-details.model';

@Injectable({
  providedIn: 'root',
})
export class GameDetailsService {
  private apiUrl = 'https://localhost:7144/api/Game';

  constructor(private http: HttpClient) {}

  // Get the game grid from the backend
  getGrid(): Observable<GameDetails> {
    return this.http.get<GameDetails>(`${this.apiUrl}/load-grid`);
  }

  // Fire a shot at a specified coordinate
  fireShot(coordinate: string): Observable<Shot> {
    // Send the coordinate as a JSON object in the request body
    return this.http.post<Shot>(`${this.apiUrl}/fire-shot`, { coordinate });
  }

  //Restart the game
  restartGame(): Observable<GameDetails> {
    return this.http.post<GameDetails>(`${this.apiUrl}/restart`, {});
  }
}
