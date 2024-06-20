# Capital_Internship
## Table of Contents
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Installation
1. Clone the repository:
   ```bash
   https://github.com/masterpiece007/Capital_Internship.git
   
2. Switching between Cosmos DB Local Emulator and Azure Environment:
      - open the appsettings.json file, located in __Capital_Internship.API__ project.
      - there are two pairs of __AccountEndpoint__ and __AccountKey__ properties in file, only a pair should be commented at a time.
   
3. Rebuild the entire Solution to restore all the nuget packages and dependencies.

 ## Usage
 1. Set the __Capital_Internship.API__ project as the startup project, then run.
 2. Navigate to your browser where the swagger page is displayed to access the available endpoints.
 3. Available endpoints are:
    - _api/Programs/CreateProgramAndApplication_
    - _api/Programs/SubmitCandidateApplication_
    - _api/Programs/GetFullProgramDetails_
    - _api/Programs/GetCandidateApplicationsByProgramId_
    - _api/Programs/GetAllPrograms_
    - _api/Programs/EditProgramQuestion_
   
  ## License
  This project is licensed under the MIT License - see the LICENSE file for details.


   

