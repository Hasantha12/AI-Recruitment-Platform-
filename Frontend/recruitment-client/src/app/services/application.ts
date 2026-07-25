import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ApplicationResponse {
  id: number;
  jobId: number;
  jobTitle: string;
  candidateId: number;
  candidateName: string;
  status: string;
  matchScore?: number;
  appliedDate: string;
}

export interface ApplyResponse {
  message: string;
  applicationId: number;
  matchScore: number;
}

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private baseUrl = 'http://localhost:5004/api/Applications';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  applyToJob(jobId: number): Observable<ApplyResponse> {
    return this.http.post<ApplyResponse>(
      this.baseUrl,
      { jobId },
      { headers: this.getHeaders() }
    );
  }

  getMyApplications(): Observable<ApplicationResponse[]> {
    return this.http.get<ApplicationResponse[]>(`${this.baseUrl}/my`, { headers: this.getHeaders() });
  }

  getApplicationsForJob(jobId: number): Observable<ApplicationResponse[]> {
    return this.http.get<ApplicationResponse[]>(`${this.baseUrl}/job/${jobId}`, { headers: this.getHeaders() });
  }

  updateStatus(applicationId: number, status: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/${applicationId}/status`, { status }, { headers: this.getHeaders() });
  }
}