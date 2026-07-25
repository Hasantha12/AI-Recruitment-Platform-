import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface JobModel {
  id: number;
  title: string;
  description: string;
  requiredSkills?: string;
  location?: string;
  status: string;
  recruiterName: string;
  postedDate: string;
  applicationCount: number;
  matchScore?: number;
}

export interface CreateJobRequest {
  title: string;
  description: string;
  requiredSkills?: string;
  location?: string;
  organizationId: number;
}

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private baseUrl = 'http://localhost:5004/api/Jobs';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getAllJobs(): Observable<JobModel[]> {
    return this.http.get<JobModel[]>(this.baseUrl);
  }

  getRecommendedJobs(): Observable<JobModel[]> {
    return this.http.get<JobModel[]>(`${this.baseUrl}/recommended`, { headers: this.getHeaders() });
  }

  getJobById(id: number): Observable<JobModel> {
    return this.http.get<JobModel>(`${this.baseUrl}/${id}`);
  }

  createJob(data: CreateJobRequest): Observable<any> {
    return this.http.post(this.baseUrl, data, { headers: this.getHeaders() });
  }

  closeJob(id: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}/close`, {}, { headers: this.getHeaders() });
  }

  deleteJob(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, { headers: this.getHeaders() });
  }
}