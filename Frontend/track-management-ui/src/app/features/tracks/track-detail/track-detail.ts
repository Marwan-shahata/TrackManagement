import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnInit
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TrackDetails } from '../models/track-details.model';
import { TrackApiService } from '../services/track-api.service';

@Component({
  selector: 'app-track-detail',
  imports: [CommonModule],
  templateUrl: './track-detail.html',
  styleUrl: './track-detail.css'
})
export class TrackDetail implements OnInit {

  track: TrackDetails | null = null;

  loading = true;

  errorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly trackApiService: TrackApiService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!id) {
      this.errorMessage = 'Invalid track id.';
      this.loading = false;
      return;
    }

    this.loadTrack(id);
  }

  loadTrack(id: number): void {
    this.loading = true;
    this.errorMessage = '';

    this.trackApiService
      .getTrackById(id)
      .subscribe({
        next: (track) => {
          this.track = track;
          this.loading = false;

          this.cdr.markForCheck();
        },

        error: () => {
          this.errorMessage = 'Track not found.';
          this.loading = false;

          this.cdr.markForCheck();
        }
      });
  }

  goBack(): void {
    this.router.navigate(['/tracks']);
  }
}
