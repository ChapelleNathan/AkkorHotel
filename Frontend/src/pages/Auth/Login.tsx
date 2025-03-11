import { FormEvent, useState } from "react";
import { Button, Form, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import { ConnectUserDto } from "../../Dto/UserDto";
import axios from "axios";
import { useNavigate } from "react-router";

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const connect = async (event: FormEvent) => {
        event.preventDefault();
        let connectUserDto: ConnectUserDto = new ConnectUserDto(email, password);
        try {
            const jwt = (await axios.post('http://localhost:8080/login', connectUserDto)).data.response as string 
            localStorage.setItem('bearer', jwt);
            navigate('/');
        } catch (error) {
            console.log(error);
        }
    } 

    return(
        <>
            <h1 className="text-center">Connexion</h1>
            <Form className="col-4 offset-4 d-flex flex-column align-items-center" onSubmit={connect}>
                <FormGroup className="col-12">
                    <FormLabel>Email</FormLabel>
                    <FormControl className="cy-email" type="text" placeholder="Email" onChange={e => setEmail(e.target.value)}/>
                    <FormLabel>Mot de Passe</FormLabel>
                    <FormControl  className="cy-password" type="password" placeholder="Mot de Passe" onChange={e => setPassword(e.target.value)}/>
                </FormGroup>
                <Button className="mt-2 cy-button" type="submit" variant="primary">Se connecter</Button>
            </Form>
        </>
    )
}