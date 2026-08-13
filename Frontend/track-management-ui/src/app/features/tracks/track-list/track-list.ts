import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Track } from '../models/track.model';
import { TrackApiService } from '../services/track-api.service';
import { Router } from '@angular/router';
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
    private readonly trackApiService: TrackApiService,
      private readonly cdr: ChangeDetectorRef,
  private readonly router: Router

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

        this.cdr.markForCheck();
      },

      error: () => {
        this.errorMessage = 'Failed to load tracks.';
        this.loading = false;

        this.cdr.markForCheck();
      }
    });
}

  onStatusChange(): void {
    this.loadTracks();
  }

  openTrack(id: number): void {
  this.router.navigate(['/tracks', id]);
  }
  
}
