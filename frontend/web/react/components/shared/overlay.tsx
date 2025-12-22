import React from 'react';

export default function Overlay({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <>
      <div className="fixed inset-0 z-1000 bg-black opacity-60"></div>
      <div className="fixed inset-0 z-1001 flex items-center justify-center">{children}</div>
    </>
  );
}
