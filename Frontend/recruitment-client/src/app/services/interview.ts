import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface InterviewResponse {
  id: number;
  applicationId: number;
  candidateName: string;
  jobTitle: string;
  scheduledTime: string;
  interviewerName: string;
  mode: string;
  notes: string;
  status: string;
}

export interface ScheduleInterview {
  applicationId: number;
  scheduledTime: string;
  mode: string;
  notes: string;
}

@Injectable({
  providedIn: 'root'
})
export class InterviewService {

  private apiUrl = 'http://localhost:5004/api/Interviews';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  scheduleInterview(data: ScheduleInterview): Observable<any> {
    return this.http.post(
      this.apiUrl,
      data,
      { headers: this.getHeaders() }
    );
  }

  getApplicationInterviews(applicationId: number): Observable<InterviewResponse[]> {
    return this.http.get<InterviewResponse[]>(
      `${this.apiUrl}/application/${applicationId}`,
      { headers: this.getHeaders() }
    );
  }

  getMyInterviews(): Observable<InterviewResponse[]> {
    return this.http.get<InterviewResponse[]>(
      `${this.apiUrl}/my`,
      { headers: this.getHeaders() }
    );
  }

  getCandidateInterviews(candidateId: number): Observable<InterviewResponse[]> {
    return this.http.get<InterviewResponse[]>(
      `${this.apiUrl}/candidate/${candidateId}`,
      { headers: this.getHeaders() }
    );
  }

  updateInterview(id: number, data: ScheduleInterview): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/${id}`,
      data,
      { headers: this.getHeaders() }
    );
  }

  deleteInterview(id: number): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/${id}`,
      { headers: this.getHeaders() }
    );
  }

}