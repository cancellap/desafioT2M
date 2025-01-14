import React from "react";
import { BrowserRouter, useLocation } from "react-router-dom";
import AppRoute from "./Routes/AppRoute";
import NavBar from "./Components/Navbar/Navbar";

function App() {
  const location = useLocation();

   const hideNavBarRoutes = ["/login", "/cadastro"];
  const hideNavBar = hideNavBarRoutes.includes(location.pathname);

  return (
    <>
      {!hideNavBar && <NavBar />}
      <AppRoute />
    </>
  );
}

export default function RootApp() {
  return (
    <BrowserRouter>
      <App />
    </BrowserRouter>
  );
}
