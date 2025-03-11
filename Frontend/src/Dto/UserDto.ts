export class UserDto {
    id: number
    firstname: string
    lastname: string
    pseudo: string
    role: "Admin" | "User"

    constructor(id: number, firstname: string, lastname:string, pseudo: string, role: 'Admin' | 'User'){
        this.id = id;
        this.firstname = firstname;
        this.lastname = lastname;
        this.pseudo = pseudo;
        this.role = role;
    }
}

export class CreateUserDto {
    firstname: string
    lastname: string
    pseudo: string
    email: string
    password: string


    constructor(firstname: string, lastname: string, pseudo: string, email: string, password: string) {
        this.firstname = firstname;
        this.lastname = lastname;
        this.pseudo = pseudo;
        this.email = email;
        this.password = password;
    }
}