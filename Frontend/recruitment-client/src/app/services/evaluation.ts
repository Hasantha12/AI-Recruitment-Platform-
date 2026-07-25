import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EvaluationResponse {
  id: number;
  applicationId: number;
  candidateName: string;
  evaluatorName: string;
  score?: number;
  feedback?: string;
  decision?: string;
  evaluatedAt: string;
}

export interface DashboardStats {
  totalEvaluations: number;
  pendingApplications: number;
  hiredApplications: number;
  rejectedApplications: number;
  averageScore: number;
}

@Injectable({
  providedIn: 'root'
})
export class EvaluationService {

  private baseUrl = 'http://localhost:5004/api/Evaluations';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  submitEvaluation(
    applicationId: number,
    score: number,
    feedback: string,
    decision: string
  ): Observable<any> {

    return this.http.post(
      this.baseUrl,
      {
        applicationId,
        score,
        feedback,
        decision
      },
      {
        headers: this.getHeaders()
      }
    );
  }

  getEvaluations(applicationId: number): Observable<EvaluationResponse[]> {

    return this.http.get<EvaluationResponse[]>(
      `${this.baseUrl}/application/${applicationId}`,
      {
        headers: this.getHeaders()
      }
    );
  }

  // ==========================================
  // Dashboard Statistics
  // ==========================================

  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(
      `${this.baseUrl}/dashboard-stats`,
      {
        headers: this.getHeaders()
      }
    );
  }
}