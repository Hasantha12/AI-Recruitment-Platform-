import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


// ===============================
// Candidate Profile Data
// ===============================

export interface CandidateProfileData {

  id: number;

  userId: number;

  fullName: string;


  // Professional Profile

  headline?: string;

  phone?: string;

  address?: string;

  education?: string;

  certifications?: string;


  // Profile Image

  profileImage?: string;


  // AI Resume Data

  skills?: string;

  experienceYears?: number;

  resumeUrl?: string;

}





// ===============================
// Resume Upload Response
// ===============================

export interface ResumeUploadResult {

  message: string;

  extractedSkills: string[];

  extractedExperienceYears?: number;

  resumeUrl: string;

}





// ===============================
// Profile Image Upload Response
// ===============================

export interface ProfileImageUploadResult {

  message: string;

  imageUrl: string;

}







@Injectable({
  providedIn: 'root'
})
export class CandidateProfile {



  private baseUrl =
    'http://localhost:5004/api/CandidateProfile';




  constructor(
    private http: HttpClient
  ) {}







  // ===============================
  // JWT Headers
  // ===============================

  private getHeaders(): HttpHeaders {


    const token =
      localStorage.getItem('token');



    return new HttpHeaders({

      Authorization:
      `Bearer ${token}`

    });

  }










  // ===============================
  // GET MY PROFILE
  // GET api/CandidateProfile/me
  // ===============================


  getMyProfile():

  Observable<CandidateProfileData> {


    return this.http.get<CandidateProfileData>(


      `${this.baseUrl}/me`,


      {

        headers:
        this.getHeaders()

      }


    );


  }












  // ===============================
  // UPDATE PROFILE
  // PUT api/CandidateProfile/me
  // ===============================


  updateMyProfile(data:
  {

    headline?: string;

    phone?: string;

    address?: string;

    education?: string;

    certifications?: string;

    experienceYears?: number;


  }): Observable<any> {



    return this.http.put(


      `${this.baseUrl}/me`,


      data,


      {

        headers:
        this.getHeaders()

      }


    );


  }












  // ===============================
  // UPLOAD RESUME
  // POST api/CandidateProfile/upload-resume
  // ===============================


  uploadResume(
    file: File
  ): Observable<ResumeUploadResult> {



    const formData =
    new FormData();



    formData.append(
      'file',
      file
    );




    return this.http.post<ResumeUploadResult>(


      `${this.baseUrl}/upload-resume`,


      formData,


      {

        headers:
        this.getHeaders()

      }


    );


  }












  // ===============================
  // UPLOAD PROFILE IMAGE
  // POST api/CandidateProfile/upload-profile-image
  // ===============================


  uploadProfileImage(
    file: File
  ): Observable<ProfileImageUploadResult> {



    const formData =
    new FormData();



    formData.append(

      'file',

      file

    );





    return this.http.post<ProfileImageUploadResult>(



      `${this.baseUrl}/upload-profile-image`,



      formData,



      {

        headers:
        this.getHeaders()

      }



    );


  }



}