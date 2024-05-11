Software to handle projects that includes engineers and their tasks, which dependencies between tasks.
The project is build according to the 3-tier architecture:\
**DAL :**\
Dal-facade: Including the Api and the entities of dal layer
Dal-List and Dal-xml: Crud methods to the databases: lists or xmlfiles,  respectively.

**BL:**\
Complex algorithms: creating milestons of the project, based on the dates of tasks, Scheduling and calculating the dates.

**PL:**\
In WPF. Entry option for manager and engineer, Presentation of data in tables, in a clear and pleasing to the eye design.\
Note: This layer is still under development, so not all options are available.

In this project, design patterns were used: Singelton and Simple Factory.
