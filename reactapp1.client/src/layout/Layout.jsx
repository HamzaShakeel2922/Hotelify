import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import { Footer, Navbar, Sidebar } from "../components";

const Layout = () => {
  const [isSidebarOpen, setisSidebarOpen] = useState(false);
  return (
    <>
      <Navbar setisSidebarOpen={setisSidebarOpen} />
      <div className="relative px-0">
        {isSidebarOpen && <Sidebar />}
        <Outlet />
      </div>
      <Footer />
    </>
  );
};

export default Layout;
