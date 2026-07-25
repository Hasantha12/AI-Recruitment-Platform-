import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import {
  Admin,
  AdminDashboardStats,
  UserResponse,
  OrganizationResponse
} from '../services/admin';

import { Auth } from '../services/auth';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss'
})
export class AdminDashboard implements OnInit {

  stats!: AdminDashboardStats;

  users: UserResponse[] = [];

  organizations: OrganizationResponse[] = [];

  filteredUsers: UserResponse[] = [];

  searchTerm = '';

  isLoading = true;

  successMessage = '';

  errorMessage = '';

  userName = '';

  constructor(
    private adminService: Admin,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    this.userName =
      localStorage.getItem('fullName') || 'Admin';

    this.loadDashboard();

  }

  loadDashboard(): void {

    this.isLoading = true;

    this.adminService.getDashboard().subscribe({

      next: (data) => {

        this.stats = data;

        this.cdr.detectChanges();

      }

    });

    this.adminService.getUsers().subscribe({

      next: (data) => {

        this.users = data;

        this.filteredUsers = [...data];

        this.cdr.detectChanges();

      }

    });

    this.adminService.getOrganizations().subscribe({

      next: (data) => {

        this.organizations = data;

        this.isLoading = false;

        this.cdr.detectChanges();

      },

      error: () => {

        this.errorMessage = 'Failed to load dashboard.';

        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }

  searchUsers(): void {

    const value = this.searchTerm.toLowerCase();

    this.filteredUsers = this.users.filter(user =>
      user.fullName.toLowerCase().includes(value) ||
      user.email.toLowerCase().includes(value) ||
      user.role.toLowerCase().includes(value)
    );

  }

  activate(id: number): void {

    this.adminService.activateUser(id).subscribe({

      next: () => {

        this.successMessage = 'User activated successfully.';

        this.loadDashboard();

      },

      error: () => {

        this.errorMessage = 'Unable to activate user.';

      }

    });

  }

  deactivate(id: number): void {

    this.adminService.deactivateUser(id).subscribe({

      next: () => {

        this.successMessage = 'User deactivated successfully.';

        this.loadDashboard();

      },

      error: () => {

        this.errorMessage = 'Unable to deactivate user.';

      }

    });

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}