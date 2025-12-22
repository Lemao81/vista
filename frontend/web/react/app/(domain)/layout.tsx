import React from 'react';
import AppBar from '@/components/layout/app-bar';
import SideNavigation from '@/components/layout/side-navigation';
import PageContainer from '@/components/layout/page-container';
import ModalProvider from '@/components/shared/modal-provider';

export default function DomainLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <ModalProvider>
      <div className="flex min-h-screen flex-col">
        <AppBar />
        <div className="flex grow">
          <SideNavigation />
          <PageContainer>{children}</PageContainer>
        </div>
      </div>
    </ModalProvider>
  );
}
