import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { JobService, JobModel } from '../services/job';
import { ApplicationService, ApplicationResponse } from '../services/application';
import {
  EvaluationService,
  DashboardStats
} from '../services/evaluation';
import { Auth } from '../services/auth';

@Component({
  selector: 'app-hiring-manager-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './hiring-manager-dashboard.html',
  styleUrl: './hiring-manager-dashboard.scss'
})
export class HiringManagerDashboard implements OnInit {

  jobs: JobModel[] = [];
  applications: ApplicationResponse[] = [];
  selectedJobId: number | null = null;

  isLoading = true;
  errorMessage = '';
  successMessage = '';
  userName = '';

  evalScore: { [key: number]: number } = {};
  evalFeedback: { [key: number]: string } = {};

  // ==========================
  // Dashboard Statistics
  // ==========================

  stats: DashboardStats = {
    totalEvaluations: 0,
    pendingApplications: 0,
    hiredApplications: 0,
    rejectedApplications: 0,
    averageScore: 0
  };

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private evaluationService: EvaluationService,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    this.userName =
      localStorage.getItem('fullName') || 'Hiring Manager';

    this.loadJobs();

    this.loadDashboardStats();
  }

  loadDashboardStats(): void {

    this.evaluationService
      .getDashboardStats()
      .subscribe({

        next: (data) => {

          this.stats = data;

          this.cdr.detectChanges();

        },

        error: (err) => {

          console.error(err);

        }

      });

  }

  loadJobs(): void {

    this.isLoading = true;

    this.jobService.getAllJobs().subscribe({

      next: (data) => {

        this.jobs = data;

        this.isLoading = false;

        this.cdr.detectChanges();

      },

      error: () => {

        this.errorMessage = 'Failed to load jobs.';

        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }

  viewApplications(jobId: number): void {

    this.selectedJobId = jobId;

    this.applicationService
      .getApplicationsForJob(jobId)
      .subscribe({

        next: (data) => {

          this.applications = data;

          this.errorMessage = '';

          this.cdr.detectChanges();

        },

        error: () => {

          this.errorMessage =
            'Failed to load applications.';

          this.cdr.detectChanges();

        }

      });

  }

  submitDecision(
    applicationId: number,
    decision: string
  ): void {

    const score =
      this.evalScore[applicationId] || 0;

    const feedback =
      this.evalFeedback[applicationId] || '';

    this.evaluationService
      .submitEvaluation(
        applicationId,
        score,
        feedback,
        decision
      )
      .subscribe({

        next: () => {

          this.successMessage =
            `Decision "${decision}" submitted successfully.`;

          this.errorMessage = '';

          this.loadDashboardStats();

          if (this.selectedJobId != null) {

            this.viewApplications(this.selectedJobId);

          }

          this.cdr.detectChanges();

        },

        error: (err) => {

          console.error(err);

          this.successMessage = '';

          this.errorMessage =
            err?.error?.message ||
            'Failed to submit evaluation.';

          this.cdr.detectChanges();

        }

      });

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}