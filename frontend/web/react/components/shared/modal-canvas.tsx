import type React from 'react';

export default function ModalCanvas({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <div
      className="rounded-3xl bg-gray-100 p-8 shadow-2xl"
      onClick={(e) => e.stopPropagation()}
      onKeyDown={(e) => e.stopPropagation()}
      role="dialog"
    >
      {children}
    </div>
  );
}
