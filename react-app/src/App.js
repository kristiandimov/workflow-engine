
import './App.css';
import React , {useContext} from 'react';
import {BrowserRouter ,Route, Routes} from "react-router-dom";
import Layout from './Pages/Layout';
import Login from './Pages/Login';
import LoginV2 from './Pages/LoginV2';
import Users from './Pages/Users/Users';
import Profile from './Pages/Profile/Profile';
import Home from './Pages/Home';
import Flow from './Pages/Flow/Flow';
import Navbar from './components/Navbar';
import ProtectedRoute from './utils/ProtectedRoute';

const App = () => {
  return (
    <div className="App">
        <Routes>
            <Route path='/' element={<Layout/>}>
              <Route path="/" element={<Home />} exact ></Route>
              <Route path="loginv2" element={<LoginV2 />}></Route>
              <Route path="users" element={<ProtectedRoute><Users /></ProtectedRoute>} ></Route>           
              <Route path="profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} ></Route>           
              <Route path="flow" element={<ProtectedRoute><Flow /></ProtectedRoute>} ></Route>           
            </Route>
      </Routes>
  </div>
  );
}

export default App;
