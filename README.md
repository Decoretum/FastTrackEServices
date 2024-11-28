# FastTrackEServices

FastTrackEServices is a backend application which utilizes C# and .NET 7 in order to provide the means for a shoe store to manage its transactions and appointments regarding shoeware. <br>

## Context
The client is a conceptual shoe store owner of a store named **Fast Track** who wishes to manage his/her shoeware transactions due to an increase of percentages of teens and young adults who utilize online shopping and browsing. <br>

## Architecture and Pattern
The application utilizes the _REST architecture_ and _Bridge software design pattern_. <br><br>
![FastTrackEServicesArchitecture-Bridge Pattern drawio](https://github.com/user-attachments/assets/4880acfc-d33d-4201-adac-75d57ff41a87)

## Database Design
![FastTrackEServicesArchitecture-DataBase drawio](https://github.com/user-attachments/assets/2731a4c6-6cfb-456a-afec-1cf2b5028d04)

## Components
### Application Database Context
The context will act as a mediator between the application and the relational database management system to be used

### Models
The models represent the entities that will be subject to the application's operations. These models contain attributes wrapped inside the class in order to describe the characteristics of the models. These models will be mapped to the database models in the relational database management system. <br><br>

#### ControllerModelOwner
These models are the owner of a one-to-one or one-to-many relationship. These models do not have a reference to their many-counterpart. <br>

#### ControllerModelNotOwner
These models do not own a one-to-one or one-to-many relationship, and these contian references to their one-counterpart. <br>

### ControllerModelOwnerWithArray
These models are identical to the _ControllerModelOwner_ models, but these are also involved with another one-to-many relationship. Their many-counterpart is a model used to express an array of a characteristic of the _ControllerModelOwnerWithArray_ model. Hence, this type of model is also on the _one_ side of a one-to-many relationship.
  
### Controllers
The controllers will contain the business logic for the application's operations. The controller will also act as the _abstraction_ classes which will contain the references to the implementation's interfaces. <br>

### Implementation Modules
These modules contain both the implementation interface and the concrete implementation operations that will be used by the abstraction classes for the application's operations. <br>

### Tech Stack
C# and .NET 7 were used for the entirety of the application's backend, and MySQL was used as the relational database management system.
