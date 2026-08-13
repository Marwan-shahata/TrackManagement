import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Track } from '../models/track.model';
import { TrackApiService } from '../services/track-api.service';

@Component({
  selector: 'app-track-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './track-list.html',
  styleUrl: './track-list.css'
})
export class TrackList implements OnInit {
  tracks: Track[] = [];
  loading = true;
  errorMessage = '';

  selectedStatus = '';

  readonly statuses = [
    '',
    'Draft',
    'Submitted',
    'Distributed'
  ];

  constructor(
    private readonly trackApiService: TrackApiService
  ) {}

  ngOnInit(): void {
    this.loadTracks();
  }

  loadTracks(): void {
    this.loading = true;
    this.errorMessage = '';

    this.trackApiService
      .getTracks(this.selectedStatus || undefined)
      .subscribe({
        next: (tracks) => {
          this.tracks = tracks;
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to load tracks.';
          this.loading = false;
        }
      });
  }

  onStatusChange(): void {
    this.loadTracks();
  }
}
