import { FormEvent, use, useState } from "react";
import { Button, Form, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { CreateUserDto, UserDto } from "../../Dto/UserDto";
import axios from "axios";
import { useNavigate } from "react-router";

export default function Register() {

    const [fistname, setFirstName] = useState('');
    const [lastname, setLastName] = useState('');
    const [pseudo, setPseudo] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const createUser = async (event: FormEvent) => {
        event.preventDefault();
        let createUserDto: CreateUserDto = new CreateUserDto(fistname, lastname, pseudo, email, password);
        try {
            (await axios.post('http://localhost:8080/api/user', createUserDto)).data as UserDto;
            navigate('/')
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <>
            <h1 className="text-center">Connexion</h1>
            <Form className="col-4 offset-4" onSubmit={createUser}>
                <FormGroup>
                    <FormLabel>Nom</FormLabel>
                    <FormControl type="text" placeholder="Nom" onChange={e => setLastName(e.target.value)}/>
                    <FormLabel>Prénom</FormLabel>
                    <FormControl type="text" placeholder="Prénom" onChange={e => setFirstName(e.target.value)}/>
                    <FormLabel>Pseudo</FormLabel>
                    <FormControl type="text" placeholder="Pseudo" onChange={e => setPseudo(e.target.value)}/>
                    <FormLabel>Email</FormLabel>
                    <FormControl type="email" placeholder="Email" onChange={e => setEmail(e.target.value)}/>
                    <FormLabel>Mot de Passe</FormLabel>
                    <FormControl type="password" placeholder="mot de passe" onChange={e => setPassword(e.target.value)}/>
                </FormGroup>
                <Button variant="primary" type="submit">S'inscrire</Button>
            </Form>
        </>
    )
}