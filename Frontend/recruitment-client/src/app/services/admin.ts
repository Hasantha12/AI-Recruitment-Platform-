import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AdminDashboardStats {
  totalUsers: number;
  totalCandidates: number;
  totalRecruiters: number;
  totalHiringManagers: number;
  totalOrganizations: number;
  totalJobs: number;
  openJobs: number;
  totalApplications: number;
}

export interface UserResponse {
  id: number;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
  organizationName: string;
}

export interface OrganizationResponse {
  id: number;
  name: string;
  department: string;
  userCount: number;
  jobCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class Admin {

  private baseUrl = 'http://localhost:5004/api/Admin';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getDashboard(): Observable<AdminDashboardStats> {
    return this.http.get<AdminDashboardStats>(
      `${this.baseUrl}/dashboard`,
      { headers: this.getHeaders() }
    );
  }

  getUsers(): Observable<UserResponse[]> {
    return this.http.get<UserResponse[]>(
      `${this.baseUrl}/users`,
      { headers: this.getHeaders() }
    );
  }

  activateUser(id: number): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/users/${id}/activate`,
      {},
      { headers: this.getHeaders() }
    );
  }

  deactivateUser(id: number): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/users/${id}/deactivate`,
      {},
      { headers: this.getHeaders() }
    );
  }

  getOrganizations(): Observable<OrganizationResponse[]> {
    return this.http.get<OrganizationResponse[]>(
      `${this.baseUrl}/organizations`,
      { headers: this.getHeaders() }
    );
  }

}