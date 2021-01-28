import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilityService {

  constructor() { }

  titleCase(word: string) {
    if (!word) { return word; }
    return word[0].toUpperCase() + word.substr(1).toLowerCase();
  }

  addDate(date: Date, days: number) {
    if (date === undefined) {
      date = new Date();
    }
    date.setDate(date.getDate() + days);
    return date.toISOString().substring(0, 10);
  }
  currentDate() {
    // const currentDate = new Date();
    // const day = currentDate.getDate();
    // const month = currentDate.getMonth() + 1;
    // const year = currentDate.getFullYear();
    // return month + '/' + day + '/' + year;
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10);
  }

}
