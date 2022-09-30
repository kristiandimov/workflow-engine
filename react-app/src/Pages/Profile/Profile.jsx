import { Button,Table } from "react-bootstrap";
import React,{useEffect, useState,useContext} from "react";
import AuthContext from "../../context/AuthContext";
import useAxios from "../../utils/useAxios";

const Profile = () => {
    const[edit,setEdit] = useState(true);
    let {user} = useContext(AuthContext)
    const[userData,setUserData] = useState({});
    const[history,setHistory] = useState([]);

    let api = useAxios();

    const handleSumbit = () =>{

            const data = {
                'id':Number(user.LoggedUserId),
                'username':username ? username : userData.username,
                'firstName':firstName ? firstName : userData.firstName,
                'lastName':lastName ? lastName : userData.lastName,
                'password':password ? password : userData.password,
                'email':email ? email : userData.email,
                'phone':phone ? phone : userData.phone
            }

        updateUser(data);
    }

    const getUsers = async () => {
        let response = await api.get("/api/Users")
        .then(response => {
            let data = response.data.data;
            setUserData(data.filter(u => u.id == user.LoggedUserId)[0]);           
        })
    }

    const getHistory = async () => {
        let response = await api.get('/api/History?id=' + Number(user.LoggedUserId))
        .then(response => {
            let data = response.data.data;
            setHistory(data);           
        })
    }
         
    const updateUser = async (data) =>{
        let response = await api.post("/api/Users",data)
        .then(response => {
            let data  = response.data.data;
           setUserData(data);
        });
        editable();

    }

    const editable = () =>
    {
        setEdit(!edit);
    }

    const [username,setUsername] = useState('');
    const [phone,setPhone] = useState('');
    const [email,setEmail] = useState('');
    const [firstName,setFirstName] = useState('');
    const [lastName,setLastName] = useState('');
    const [password,setPassword] = useState('');

    useEffect(() => {
        getUsers();  
        getHistory();     
    },[]);

    return(
        <div class="container">
            <div class="mt-5">
                <h1>User Profile</h1>
            </div>
                <div class="main-body mt-5">
                    <div class="row">
                        <div class="col-lg-4">
                            <div class="card">
                                <div class="card-body">
                                    <div class="d-flex flex-column align-items-center text-center">
                                        <img src="https://bootdey.com/img/Content/avatar/avatar6.png" alt="Admin" class="rounded-circle p-1 bg-primary" width="110"/>
                                        <div class="mt-3">
                                            <h4>{userData.firstName + " " + userData.lastName}</h4>
                                            <p class="text-secondary mb-1">Developer</p>                                   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-8" onSubmit={handleSumbit}>
                            <div class="card">
                                <div class="card-body">
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">First Name</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="text" 
                                                class="form-control" 
                                                disabled = {edit}  
                                                onChange={(e) => setFirstName(e.target.value)} 
                                                defaultValue = {userData.firstName}/>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">Last Name</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="text" 
                                                class="form-control" 
                                                disabled = {edit}  
                                                onChange={(e) => setLastName(e.target.value)} 
                                                defaultValue = {userData.lastName}/>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">Username</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="text" 
                                                class="form-control" 
                                                disabled = {edit} 
                                                onChange={(e) => setUsername(e.target.value)} 
                                                defaultValue = {userData.username}/>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">Password</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="password" 
                                                class="form-control" 
                                                disabled = {edit} 
                                                onChange={(e) => setPassword(e.target.value)} 
                                                defaultValue = {userData.password}/>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">Phone</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="text" 
                                                class="form-control" 
                                                disabled = {edit} 
                                                onChange={(e) => setPhone(e.target.value)} 
                                                defaultValue = {userData.phone}/>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0">Email</h6>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <input 
                                                type="text" 
                                                class="form-control" 
                                                disabled = {edit} 
                                                onChange={(e) => setEmail(e.target.value)} 
                                                defaultValue = {userData.email}/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3"></div>
                                        <div class="col-sm-9 text-secondary">
                                            <Button 
                                                className="btn btn-primary px-4"
                                                onClick={editable}
                                                hidden = {edit === true ? false : true }
                                                >Edit</Button>
                                        </div>
                                        <div class="col-sm-9 text-secondary">
                                            <Button 
                                                className="btn btn-primary px-4"
                                                type="submit"
                                                hidden = {edit === true ? true : false }
                                                onClick={handleSumbit}
                                                >Save changes</Button>
                                        </div>
                                    </div>
                                </div>
                            </div>                           
                        </div>
                    </div>
                </div>
                <div class="mt-5">
                    <h2>Execution History</h2>
                </div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>FlowConfig</th>
                            <th>FlowResult</th>
                            <th>Status</th>
                            <th>ExecutionTime</th>
                        </tr>
                    </thead>
                    <tbody>
                    <>
                        {history.map((history) => (
                            <tr key={history.id}>
                                <td>{history.flowConfig}</td>
                                <td>{history.flowResult}</td>
                                <td>{history.status}</td>
                                <td>{history.executionTime}</td>
                            </tr>
                        ))}                  
                    </>
                    </tbody>
                </Table>
            </div>
);
}

export default Profile;