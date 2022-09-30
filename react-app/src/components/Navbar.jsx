import React ,{useContext,useState,useEffect} from "react"
import "../Navbar.css"
import AuthContext from "../context/AuthContext";
import { Link } from "react-router-dom";
import { useRadioGroup } from "@material-ui/core";

const Navbar = () =>{
    let {user , logoutUser} = useContext(AuthContext);
    let [isAdmin ,setIsAdmin] = useState(false);

    if(user){
        if(user.Role == 'admin'){
            isAdmin = true;
        }else{
            isAdmin = false;
        }
    }

    return (
        <div className="topnav">
             <a class="navbar-brand" href="/"
                ><img
                    id="MDB-logo"
                    src={"./profile.png"}
                    alt="W Logo"
                    draggable="false"
                    height="30"
                /></a>
                <button
                    class="navbar-toggler"
                    type="button"
                    data-mdb-toggle="collapse"
                    data-mdb-target="#navbarSupportedContent"
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                <i class="fas fa-bars"></i>
                </button>
                <Link to="/flow">Flows</Link>                
                { !isAdmin ? (<Link to="/profile">Profile</Link>) : (<Link to="/users" >Users</Link>)} 
                { !user ? (<Link to="/loginV2">Login</Link>) : (<Link to=""  onClick={logoutUser}>Logout</Link>)}   
                {user && <a style={{float:'right', color:"white"}}>Hello {user.FirstName + " " + user.LastName}</a>}
        </div> 
    );
}

export default Navbar;