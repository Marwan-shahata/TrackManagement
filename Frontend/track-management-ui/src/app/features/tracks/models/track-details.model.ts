import { Distribution } from './distribution.model';
import { Track } from './track.model';

export interface TrackDetails extends Track {
  distributions: Distribution[];
}
