import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { JobService, JobModel } from '../services/job';
import { ApplicationService, ApplicationResponse } from '../services/application';
import { Auth } from '../services/auth';
import { CandidateProfile, CandidateProfileData } from '../services/candidate-profile';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-candidate-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './candidate-dashboard.html',
  styleUrl: './candidate-dashboard.scss'
})
export class CandidateDashboard implements OnInit {


  jobs: JobModel[] = [];

  isLoading = true;

  errorMessage = '';

  successMessage = '';

  userName = '';

  profileCompletion = 0;



  // PROFILE

  profile: CandidateProfileData | null = null;


  profileForm = {

    headline: '',
    phone: '',
    address: '',
    education: '',
    certifications: '',
    experienceYears: 0

  };


  selectedImage: File | null = null;

  isUpdatingProfile = false;



  // RESUME

  extractedSkills: string[] = [];

  isUploading = false;

  selectedFile: File | null = null;



  // APPLICATIONS

  myApplications: ApplicationResponse[] = [];

  isLoadingApplications = true;

  appliedJobIds: Set<number> = new Set();



  constructor(

    private jobService: JobService,

    private applicationService: ApplicationService,

    private authService: Auth,

    private profileService: CandidateProfile,

    private router: Router,

    private cdr: ChangeDetectorRef

  ) {}





  ngOnInit(): void {


    this.userName =
    localStorage.getItem('fullName') || 'Candidate';


    this.loadJobs();

    this.loadProfile();

    this.loadMyApplications();


  }





  // ================= PROFILE =================


  loadProfile(): void {


    this.profileService
    .getMyProfile()

    .subscribe({

      next:(data)=>{


        this.profile = data;

        this.calculateProfileCompletion();



        this.profileForm = {


          headline: data.headline || '',


          phone: data.phone || '',


          address: data.address || '',


          education: data.education || '',


          certifications: data.certifications || '',


          experienceYears:
          data.experienceYears || 0


        };



        this.cdr.detectChanges();


      },


      error:(err)=>{

        console.log(
          "PROFILE LOAD ERROR",
          err
        );

      }


    });


  }







  updateProfile(): void {


    this.isUpdatingProfile = true;


    this.profileService
    .updateMyProfile(this.profileForm)

    .subscribe({


      next:()=>{


        this.successMessage =
        "Profile updated successfully";


        this.isUpdatingProfile=false;


        this.loadProfile();


      },


      error:(err)=>{


        console.log(err);


        this.errorMessage =
        "Profile update failed";


        this.isUpdatingProfile=false;


      }


    });



  }







  onImageSelected(event: Event): void {


    const input =
    event.target as HTMLInputElement;



    if(input.files &&
       input.files.length > 0)
    {


      this.selectedImage =
      input.files[0];


    }


  }







  uploadProfileImage(): void {


    if(!this.selectedImage)
    {

      this.errorMessage =
      "Please select image";

      return;

    }



    this.profileService
    .uploadProfileImage(this.selectedImage)

    .subscribe({


      next:(res)=>{


        this.successMessage =
        "Profile image uploaded";


        this.loadProfile();


      },


      error:(err)=>{


        console.log(err);


        this.errorMessage =
        "Image upload failed";


      }


    });



  }







  // ================= JOBS =================



  loadJobs(): void {


    this.isLoading=true;



    this.jobService
    .getRecommendedJobs()

    .subscribe({


      next:(data)=>{


        this.jobs =
        data
        .filter(j=>j.status==='Open')
        .map(job=>({
          ...job,
          matchScor: this.calculateJobMatchScore(job)
        }));


        this.isLoading=false;


        this.cdr.detectChanges();


      },


      error:()=>{


        this.jobService
        .getAllJobs()

        .subscribe({


          next:(data)=>{


            this.jobs =
            data.filter(
              j=>j.status==='Open'
            );


            this.isLoading=false;


            this.cdr.detectChanges();


          },


          error:()=>{


            this.errorMessage =
            "Failed to load jobs";


            this.isLoading=false;


          }


        });


      }


    });


  }







  // ================= RESUME =================



  onFileSelected(event:Event):void {


    const input =
    event.target as HTMLInputElement;



    if(input.files &&
       input.files.length>0)
    {


      this.selectedFile =
      input.files[0];


    }


  }







  uploadResume():void {


    if(!this.selectedFile)
    {

      this.errorMessage =
      "Please select PDF resume";


      return;

    }




    this.isUploading=true;



    this.profileService
    .uploadResume(this.selectedFile)

    .subscribe({


      next:(res)=>{


        this.isUploading=false;


        this.extractedSkills =
        res.extractedSkills;



        this.successMessage =
        `Resume analyzed. ${res.extractedSkills.length} skills found`;



        this.loadProfile();

        this.loadJobs();



      },


      error:(err)=>{


        console.log(err);


        this.isUploading=false;


        this.errorMessage =
        err.error?.message ||
        "Resume upload failed";


      }


    });



  }







  // ================= APPLICATION =================



  loadMyApplications():void {


    this.isLoadingApplications=true;



    this.applicationService
    .getMyApplications()

    .subscribe({


      next:(data)=>{


        this.myApplications=data;


        this.appliedJobIds =
        new Set(
          data.map(a=>a.jobId)
        );


        this.isLoadingApplications=false;


        this.cdr.detectChanges();


      },


      error:()=>{


        this.isLoadingApplications=false;


      }


    });



  }







  hasApplied(jobId:number):boolean{


    return this.appliedJobIds.has(jobId);


  }







  applyToJob(jobId:number):void{


    this.applicationService
    .applyToJob(jobId)

    .subscribe({


      next:(res)=>{


        this.successMessage =
        `Applied successfully. Match Score ${res.matchScore}%`;


        this.loadMyApplications();


      },


      error:(err)=>{


        this.errorMessage =
        err.error?.message ||
        "Application failed";


      }


    });


  }






calculateProfileCompletion(): void {

  let score = 0;


  if(this.profile?.headline){
    score += 15;
  }


  if(this.profile?.phone){
    score += 15;
  }


  if(this.profile?.address){
    score += 10;
  }


  if(this.profile?.education){
    score += 15;
  }


  if(this.profile?.certifications){
    score += 10;
  }


  if(this.profile?.skills){
    score += 20;
  }


  if(this.profile?.resumeUrl){
    score += 15;
  }


  this.profileCompletion = score;

}
calculateJobMatchScore(job: JobModel): number {

  let score = 0;


  const userSkills =
    this.profile?.skills
      ?.toLowerCase()
      .split(',')
      || [];



  const jobSkills =
    job.requiredSkills
      ?.toLowerCase()
      .split(',')
      || [];



  jobSkills.forEach(skill => {

    if(
      userSkills.some(
        userSkill =>
          userSkill.trim() === skill.trim()
      )
    ){

      score += 20;

    }

  });



  if(this.profile?.education){

    score += 10;

  }



  if(this.profile?.experienceYears){

    score += Math.min(
      this.profile.experienceYears * 5,
      20
    );

  }



  if(score > 100){

    score = 100;

  }



  return score;

}  logout():void{


    this.authService.logout();


    this.router.navigate(['/login']);


  }


}