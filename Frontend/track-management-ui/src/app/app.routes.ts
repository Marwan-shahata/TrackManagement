import { Routes } from '@angular/router';
import { TrackList } from './features/tracks/track-list/track-list';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'tracks',
    pathMatch: 'full'
  },
  {
    path: 'tracks',
    component: TrackList
  }
];
