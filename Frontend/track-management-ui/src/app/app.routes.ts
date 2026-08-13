import { Routes } from '@angular/router';

import { TrackList } from './features/tracks/track-list/track-list';
import { TrackDetail } from './features/tracks/track-detail/track-detail';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'tracks',
    pathMatch: 'full'
  },
  {
    path: 'tracks',
    component: TrackList
  },
  {
    path: 'tracks/:id',
    component: TrackDetail
  }
];
