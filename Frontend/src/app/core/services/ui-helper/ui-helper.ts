import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UiHelperService {

formatDuration(durationInSeconds: number | null): string {
  if (durationInSeconds === null || durationInSeconds === undefined) return '';

  let seconds = durationInSeconds;

  const weeks = Math.floor(seconds / (7 * 24 * 60 * 60));
  seconds %= 7 * 24 * 60 * 60;

  const days = Math.floor(seconds / (24 * 60 * 60));
  seconds %= 24 * 60 * 60;

  const hours = Math.floor(seconds / (60 * 60));
  seconds %= 60 * 60;

  const minutes = Math.floor(seconds / 60);
  seconds %= 60;

  const result: string[] = [];

  if (weeks > 0) result.push(`${weeks} week${weeks > 1 ? 's' : ''}`);
  if (days > 0) result.push(`${days} day${days > 1 ? 's' : ''}`);
  if (hours > 0) result.push(`${hours} hour${hours > 1 ? 's' : ''}`);
  if (minutes > 0) result.push(`${minutes} minute${minutes > 1 ? 's' : ''}`);

  if (weeks === 0 && days === 0 && hours === 0 && minutes === 0 && seconds > 0) {
    return `${seconds} second${seconds > 1 ? 's' : ''}`;
  }

  return result.length ? result.join(' ') : `0 seconds`;
}

  highlight(text: string, searchTerm: string): string {
    if (!searchTerm) return text;

    const regex = new RegExp(`(${searchTerm})`, 'gi');

    return text.replace(
      regex,
      `<mark class="bg-yellow-300 text-white px-1 rounded">$1</mark>`
    );
  }
}
