import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Employee, Routine, RoutineList } from '../_models/employee';

@Injectable({
  providedIn: 'root'
})
export class HumanResourceService {
  baseUrl = environment.apiUrl + '';
  constructor(private http: HttpClient) { }

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.baseUrl + 'employees');
  }
  getEmployee(id): Observable<Employee> {
    return this.http.get<Employee>(this.baseUrl + 'employees/' + id);
  }
  getNextEmployeeNo() {
    return this.http.get(this.baseUrl + 'employees/GetNextEmployeeNo').pipe(
      map(response => {
        return <number>response;
      }));
  }
  CreateEmployee(employee: Employee) {
    return this.http.post(this.baseUrl + 'employees', employee);
  }
  updateEmployee(id: number, employee: Employee) {
    return this.http.put(this.baseUrl + 'employees/' + id, employee);
  }
  patchEmployee(id: number, isNotActive: any) {
    let params = new HttpParams();
    params = params.append('isNotActive', isNotActive);
    return this.http.patch(this.baseUrl + 'employees/' + id, { params });
  }
  deleteEmployee(id: number) {
    return this.http.delete(this.baseUrl + 'employees/' + id);
  }

  getRoutines(date: Date): Observable<Routine[]> {
    let params = new HttpParams();
    params = params.append('date', date.toString());

    return this.http.get<Routine[]>(this.baseUrl + 'routines', { params });
  }
  getroutine(id): Observable<Routine> {
    return this.http.get<Routine>(this.baseUrl + 'routines/' + id);
  }
  CreateRoutines(routines: Routine[]) {
    return this.http.post(this.baseUrl + 'routines', routines);
  }
  updateRoutines(date: Date, routines: RoutineList) {
    let params = new HttpParams();
    params = params.append('date', date.toString());
    return this.http.put(this.baseUrl + 'routines', routines, { params });
  }
  deleteRoutine(id: number) {
    return this.http.delete(this.baseUrl + 'routines/' + id);
  }
  createAutoRoutine(date: Date) {
    let params = new HttpParams();
    params = params.append('date', date.toString());
    return this.http.get(this.baseUrl + 'routines/CreateAutoRoutine', { params });
  }
}
