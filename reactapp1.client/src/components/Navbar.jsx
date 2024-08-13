import React from "react";

const Navbar = ({ setisSidebarOpen }) => {
  return (
    <div className="bg-[#7b9191] h-28 p-10">
      <button
        className="bg-blue-400"
        onClick={() => setisSidebarOpen((state) => !state)}
      >
        Sidebar
      </button>
    </div>
  );
};

export default Navbar;
