import React from 'react';

export default function PageContainer({ children }: Readonly<{ children: React.ReactNode }>) {
  return <main className="grow bg-neutral-200 p-2">{children}</main>;
}
