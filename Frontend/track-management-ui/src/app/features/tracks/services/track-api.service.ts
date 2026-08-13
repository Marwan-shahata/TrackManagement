import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';


import { Track } from '../models/track.model';
import { TrackDetails } from '../models/track-details.model';

@Injectable({
  providedIn: 'root'
})
export class TrackApiService {
  private readonly apiUrl = `${environment.apiUrl}/tracks`;

  constructor(private readonly http: HttpClient) {}

  getTracks(status?: string): Observable<Track[]> {
    let params = new HttpParams();

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<Track[]>(
      this.apiUrl,
      { params }
    );
  }

  getTrackById(id: number): Observable<TrackDetails> {
    return this.http.get<TrackDetails>(
      `${this.apiUrl}/${id}`
    );
  }
}
