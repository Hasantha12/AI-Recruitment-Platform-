import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { Auth, RegisterRequest } from '../../services/auth';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {


  fullName = '';
  email = '';
  password = '';
  role = 'Candidate';


  errorMessage = '';
  successMessage = '';

  isLoading = false;



  constructor(
    private authService: Auth,
    private router: Router
  ) {}




  onSubmit(): void {


    this.errorMessage = '';

    this.successMessage = '';

    this.isLoading = true;



    const data: RegisterRequest = {

      fullName: this.fullName,

      email: this.email,

      password: this.password,

      role: this.role

    };




    this.authService.register(data)
      .subscribe({


        next: () => {


          this.isLoading = false;


          this.successMessage =
          'Account created successfully!';



          setTimeout(() => {

            this.router.navigate(['/login']);

          },1500);



        },



        error: (err) => {


          this.isLoading = false;


          this.errorMessage =
          err.error?.message ||
          'Registration failed.';


        }


      });


  }


}