FROM python:3.7
# Avoid warnings by switching to noninteractive
ENV DEBIAN_FRONTEND=noninteractive
ARG USERNAME=python
ARG USER_UID=1000
ARG USER_GID=2000
RUN apt-get update \
    && apt-get install -y apt-utils dialog \
    && apt-get install -y git procps lsb-release \
    && groupadd --gid $USER_GID $USERNAME \
    && useradd -s /bin/bash --uid $USER_UID --gid $USER_GID -m $USERNAME \
    # Add sudo support for non-root user
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME \
    # Clean up
    && apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*
ENV DEBIAN_FRONTEND=
