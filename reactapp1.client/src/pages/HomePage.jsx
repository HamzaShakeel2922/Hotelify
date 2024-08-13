import React from "react";

const HomePage = () => {
  return (
    <div>
      <div className="text-3xl font-[Poppins] grid-cols-12 grid-rows-12 grid h-[100vh]">
        <div className="bg-[#e6bcbc] col-span-12 flex justify-evenly p-5 row-span-12">
          <div className="bg-black w-[45%] h-[100%]"></div>
          <div className="bg-neutral-500 w-[45%] h-[100%] col-start-7"></div>
        </div>
      </div>
      <div className="h-96 bg-zinc-500"></div>
      <div className="h-96 bg-pink-500"></div>
      <div className="h-96 bg-blue-500"></div>
      <div className="h-96 bg-green-100"></div>
    </div>
  );
};

export default HomePage;
