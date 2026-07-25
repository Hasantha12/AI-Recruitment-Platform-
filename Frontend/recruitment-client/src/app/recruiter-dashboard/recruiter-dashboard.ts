import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { JobService, JobModel } from '../services/job';
import { ApplicationService, ApplicationResponse } from '../services/application';
import {
  InterviewService,
  InterviewResponse,
  ScheduleInterview
} from '../services/interview';
import { Auth } from '../services/auth';

@Component({
  selector: 'app-recruiter-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './recruiter-dashboard.html',
  styleUrl: './recruiter-dashboard.scss'
})
export class RecruiterDashboard implements OnInit {

  jobs: JobModel[] = [];
  applications: ApplicationResponse[] = [];
  interviews: InterviewResponse[] = [];

  selectedJobId: number | null = null;
  selectedApplicationId = 0;

  isLoading = true;

  errorMessage = '';
  successMessage = '';

  userName = '';

  showPostForm = false;
  showInterviewForm = false;

  searchTerm = '';
  minMatchScore = 0;

  newJob = {
    title: '',
    description: '',
    requiredSkills: '',
    location: '',
    organizationId: 2
  };

  newInterview: ScheduleInterview = {
    applicationId: 0,
    scheduledTime: '',
    mode: 'Online',
    notes: ''
  };

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private interviewService: InterviewService,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.userName = localStorage.getItem('fullName') || 'Recruiter';
    this.loadJobs();
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
    togglePostForm(): void {

    this.showPostForm = !this.showPostForm;

  }

  postJob(): void {

    this.successMessage = '';

    this.errorMessage = '';

    this.jobService.createJob(this.newJob).subscribe({

      next: () => {

        this.successMessage = 'Job posted successfully!';

        this.newJob = {

          title: '',

          description: '',

          requiredSkills: '',

          location: '',

          organizationId: 1

        };

        this.showPostForm = false;

        this.loadJobs();

        this.cdr.detectChanges();

      },

      error: (err) => {

        this.errorMessage =
          err.error?.message || 'Failed to post job.';

        this.cdr.detectChanges();

      }

    });

  }

  closeJob(jobId: number): void {

    this.jobService.closeJob(jobId).subscribe({

      next: () => {

        this.successMessage = 'Job closed.';

        this.loadJobs();

        this.cdr.detectChanges();

      },

      error: () => {

        this.errorMessage = 'Failed to close job.';

        this.cdr.detectChanges();

      }

    });

  }

  viewApplications(jobId: number): void {

  console.log("VIEW CLICK:", jobId);

  this.selectedJobId = jobId;

  this.applications = [];

  this.errorMessage = "";


  this.applicationService
    .getApplicationsForJob(jobId)
    .subscribe({

      next: (data) => {

        console.log("APPLICATIONS:", data);

        this.applications = data;

        this.interviews = [];

        setTimeout(() => {
          this.cdr.detectChanges();
        });

      },


      error: (err) => {

        console.log(err);

        this.errorMessage =
        "Unable to load candidates";

      }

    });

}

  get filteredApplications(): ApplicationResponse[] {

    return this.applications.filter(app => {

      const matchesName =
        !this.searchTerm ||
        app.candidateName
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase());

      const matchesScore =
        (app.matchScore ?? 0) >= this.minMatchScore;

      return matchesName && matchesScore;

    });

  }

 openInterviewForm(applicationId: number): void {

  console.log("INTERVIEW CLICK:", applicationId);


  this.selectedApplicationId = applicationId;


  this.newInterview = {

    applicationId: applicationId,

    scheduledTime: '',

    mode: 'Online',

    notes: ''

  };


  this.showInterviewForm = true;


  setTimeout(() => {

    this.cdr.detectChanges();

  });

}
    scheduleInterview(): void {

    this.successMessage = '';
    this.errorMessage = '';

    this.interviewService
      .scheduleInterview(this.newInterview)
      .subscribe({

        next: () => {

          this.successMessage =
            'Interview scheduled successfully.';

          this.showInterviewForm = false;

          this.loadApplicationInterviews(
            this.selectedApplicationId
          );

          this.cdr.detectChanges();

        },

        error: (err) => {

          this.errorMessage =
            err.error?.message || 'Failed to schedule interview.';

          this.cdr.detectChanges();

        }

      });

  }

  loadApplicationInterviews(applicationId: number): void {

    this.interviewService
      .getApplicationInterviews(applicationId)
      .subscribe({

        next: (data) => {

          this.interviews = data;

          this.cdr.detectChanges();

        },

        error: () => {

          this.errorMessage =
            'Failed to load interviews.';

          this.cdr.detectChanges();

        }

      });

  }

  updateInterview(interview: InterviewResponse): void {

    this.selectedApplicationId =
      interview.applicationId;

    this.newInterview = {

      applicationId:
        interview.applicationId,

      scheduledTime:
        interview.scheduledTime,

      mode:
        interview.mode,

      notes:
        interview.notes

    };

    this.showInterviewForm = true;

  }

  deleteInterview(interviewId: number): void {

    if (!confirm('Cancel this interview?'))
      return;

    this.interviewService
      .deleteInterview(interviewId)
      .subscribe({

        next: () => {

          this.successMessage =
            'Interview cancelled successfully.';

          this.loadApplicationInterviews(
            this.selectedApplicationId
          );

          this.cdr.detectChanges();

        },

        error: () => {

          this.errorMessage =
            'Unable to cancel interview.';

          this.cdr.detectChanges();

        }

      });

  }
    updateApplicationStatus(appId: number, status: string): void {

    this.applicationService
      .updateStatus(appId, status)
      .subscribe({

        next: () => {

          this.successMessage = `Application ${status}.`;

          if (this.selectedJobId) {
            this.viewApplications(this.selectedJobId);
          }

          this.cdr.detectChanges();

        },

        error: (err) => {

          this.errorMessage =
            err.error?.message || 'Failed to update application status.';

          this.cdr.detectChanges();

        }

      });

  }

  resetInterviewForm(): void {

    this.showInterviewForm = false;

    this.selectedApplicationId = 0;

    this.newInterview = {

      applicationId: 0,

      scheduledTime: '',

      mode: 'Online',

      notes: ''

    };

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}
