import React from 'react';

export default function DomainLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <div className="flex min-h-screen flex-col">
      <header className="flex h-14 justify-between dark:bg-neutral-900 p-1.5">
        <div className="app-logo w-56 bg-neutral-200"></div>

        <nav className="top-nav flex gap-1">
          <a href="" className="inline-flex items-center px-4">
            Link 1
          </a>
          <a href="" className="inline-flex items-center px-4">
            Link 2
          </a>
        </nav>

        <div className="flex items-center gap-4 pr-8">
          <button className="rounded-lg bg-blue-500 px-4 py-1.5 text-white shadow-md transition-all hover:bg-blue-400">
            Sign In
          </button>
          <button className="rounded-md bg-blue-500 px-5 py-2 text-white shadow-md transition-all hover:bg-blue-400">
            Sign Up
          </button>
        </div>
      </header>

      <div className="flex grow">
        <nav className="side-nav flex w-64 flex-col gap-1 bg-neutral-800 p-1 pt-4">
          <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-700">
            Link 1
          </a>
          <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-700">
            Link 2
          </a>
          <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-700">
            Link 3
          </a>
        </nav>

        <main className="grow bg-neutral-500 p-2">{children}</main>
      </div>
    </div>
  );
}
