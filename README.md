# WPF-EvernoteClone
Basic WPF Evernote clone app that let the user to add / save notes grouped in notebooks in the personal profile.
***

## Table of contents
- [GUI](#gui)
  - [Main](#main)
  - [Login](#login)
  - [Register](#register)
  - [Notebooks](#notebooks)
    - [Rename notebooks](#rename-notebooks)
  - [Notes](#notes)
- [Main features](#main-features)
- [Technologies](#technologies)
- [Author](#author)
***

## GUI
### Main
![](/Screenshots/Main.png)
<br />At the opening of the application this will be displayed.

### Login 
![](/Screenshots/Login.png)
<br />Simple login form displayed if the user isn't authenticated.

### Register
![](/Screenshots/Register.png)
<br />Simple register form displayed if the user isn't authenticated and has clicked on register link in the login form.

### Notebooks
![](/Screenshots/Notebooks.png)
<br />If the user is authenticated, the main form could be used.

#### Rename notebooks
![](/Screenshots/Rename.png)
<br />If the user right click on a notebook name, could start the editing of the name of the notebook.

### Notes
![](/Screenshots/Notes.png)
<br />If the user select a notebook in the relative list and then click on a note, the note content will be displayed in the right editor and then the user could start to edit the content itself.
To save the data the user must click on the save button.
In the editor there is the possibility to recognize the speech, clicking the speech button.
The speech recognition is setted on italian language.

***

## Main features
This app let the user to:
* register a profile using a valid email
* login
* create notebooks linked with the personal profile (using top menu)
* create notes linked to a selected notebook (using top menu)
* add / edit content of notes
* save content of notes online
* download notes content every time user try to open it and then show the readed content in the editor
***

## Technologies
A list of technologies used within the project:
* C#
* Visual Studio 2022
* WPF
* .NET 6
* Azure speech recognition
* Firebase database
* Azure storage service
***
